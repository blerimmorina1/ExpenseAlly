<script setup lang="ts">
import { ref, onMounted, watch } from 'vue';
import { useToast } from 'primevue/usetoast';
import { TransactionService } from '@/services/TransactionsService';
import { CategoryService } from '@/services/CategoryService';

// Data references
const transactions = ref([]);
const categories = ref([]);
const transactionDialog = ref(false);
const transaction = ref({
    categoryId: '',
    type: null,
    amount: 0,
    date: new Date().toISOString().split('T')[0], // Default to current date
    notes: '',
});
const submitted = ref(false);
const toast = useToast();
const typeOptions = [
    { label: 'Income', value: 1 },
    { label: 'Expense', value: 2 },
];
const typeValidationError = ref('');

// Fetch transactions and categories on mount
onMounted(async () => {
    try {
        transactions.value = await TransactionService.getTransactions();
        categories.value = await CategoryService.getCategories(); // Fetch categories
    } catch (error) {
        toast.add({ severity: 'error', summary: 'Error', detail: 'Failed to load data', life: 3000 });
    }
});

// Watch categoryId and automatically set type when category changes
watch(
    () => transaction.value.categoryId,
    (newCategoryId) => {
        const selectedCategory = categories.value.find((cat: any) => cat.id === newCategoryId);
        if (selectedCategory) {
            transaction.value.type = selectedCategory.type; // Auto-select type based on category
            typeValidationError.value = ''; // Clear any existing validation error
        } else {
            transaction.value.type = null; // Reset if no category is selected
        }
    }
);

// Open the dialog for creating a new transaction
function openNew() {
    transaction.value = {
        categoryId: '',
        type: null,
        amount: 0,
        date: new Date().toISOString().split('T')[0],
        notes: '',
    };
    submitted.value = false;
    transactionDialog.value = true;
    typeValidationError.value = '';
}

// Close the dialog
function hideDialog() {
    transactionDialog.value = false;
    submitted.value = false;
    typeValidationError.value = '';
}

// Validate type consistency
function validateType() {
    const selectedCategory = categories.value.find((cat: any) => cat.id === transaction.value.categoryId);
    if (selectedCategory && transaction.value.type !== selectedCategory.type) {
        typeValidationError.value = `The selected type does not match the type of the chosen category (${selectedCategory.type === 1 ? 'Income' : 'Expense'}).`;
    } else {
        typeValidationError.value = ''; // Clear validation error if valid
    }
}

// Save the transaction
async function saveTransaction() {
    submitted.value = true;

    validateType();
    if (typeValidationError.value) {
        return;
    }

    if (transaction.value.categoryId && transaction.value.type !== null && transaction.value.amount > 0) {
        try {
            const newTransaction = await TransactionService.createTransaction(transaction.value);
            transactions.value.push(newTransaction);
            toast.add({ severity: 'success', summary: 'Success', detail: 'Transaction created', life: 3000 });
            transactionDialog.value = false;
        } catch (error) {
            toast.add({ severity: 'error', summary: 'Error', detail: 'Failed to create transaction', life: 3000 });
        }
    }
}

// Helper function to get the category name by ID
function getCategoryName(categoryId: string): string {
    const category = categories.value.find((cat: any) => cat.id === categoryId);
    return category ? category.name : 'Unknown';
}
</script>

<template>
    <div>
        <div class="card">
            <Toolbar class="mb-4">
                <template #start>
                    <Button label="New Transaction" icon="pi pi-plus" @click="openNew" />
                </template>
            </Toolbar>

            <DataTable :value="transactions" :paginator="true" :rows="10">
                <Column field="categoryId" header="Category" sortable>
                    <template #body="slotProps">
                        {{ getCategoryName(slotProps.data.categoryId) }}
                    </template>
                </Column>
                <Column field="type" header="Type" sortable>
                    <template #body="slotProps">
                        {{ slotProps.data.type === 1 ? 'Income' : 'Expense' }}
                    </template>
                </Column>
                <Column field="amount" header="Amount" sortable></Column>
                <Column field="date" header="Date" sortable></Column>
                <Column field="notes" header="Notes" sortable></Column>
            </DataTable>
        </div>

        <Dialog v-model:visible="transactionDialog" header="Transaction Details" :modal="true" style="width: 400px">
            <div>
                <div class="field">
                    <label for="categoryId" class="block">Category</label>
                    <Dropdown
                        id="categoryId"
                        v-model="transaction.categoryId"
                        :options="categories"
                        optionLabel="name"
                        optionValue="id"
                        :class="{ 'p-invalid': submitted && !transaction.categoryId }"
                        placeholder="Select a Category"
                        @change="validateType"
                    />
                    <small v-if="submitted && !transaction.categoryId" class="p-error">Category is required.</small>
                </div>

                <div class="field">
                    <label for="type" class="block">Type</label>
                    <Dropdown
                        id="type"
                        v-model="transaction.type"
                        :options="typeOptions"
                        optionLabel="label"
                        optionValue="value"
                        :class="{ 'p-invalid': submitted && !transaction.type }"
                        placeholder="Select a Type"
                        @change="validateType"
                    />
                    <small v-if="typeValidationError" class="p-error">{{ typeValidationError }}</small>
                </div>

                <div class="field">
                    <label for="amount" class="block">Amount</label>
                    <InputNumber
                        id="amount"
                        v-model="transaction.amount"
                        mode="currency"
                        currency="USD"
                        locale="en-US"
                        :class="{ 'p-invalid': submitted && transaction.amount <= 0 }"
                    />
                    <small v-if="submitted && transaction.amount <= 0" class="p-error">Amount must be greater than 0.</small>
                </div>

                <div class="field">
                    <label for="date" class="block">Date</label>
                    <InputText
                        id="date"
                        type="date"
                        v-model="transaction.date"
                        :class="{ 'p-invalid': submitted && !transaction.date }"
                    />
                    <small v-if="submitted && !transaction.date" class="p-error">Date is required.</small>
                </div>

                <div class="field">
                    <label for="notes" class="block">Notes</label>
                    <Textarea id="notes" v-model="transaction.notes" rows="3" cols="20" />
                </div>
            </div>

            <template #footer>
                <Button label="Cancel" icon="pi pi-times" @click="hideDialog" class="p-button-text" />
                <Button label="Save" icon="pi pi-check" @click="saveTransaction" />
            </template>
        </Dialog>
    </div>
</template>

<style>
/* Optional styles */
.field {
    margin-bottom: 1rem;
}
</style>
