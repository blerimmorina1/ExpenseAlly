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
const toast = useToast();
const typeOptions = [
    { label: 'Income', value: 1 },
    { label: 'Expense', value: 2 },
];

// Fetch transactions and categories on mount
onMounted(async () => {
    try {
        transactions.value = await TransactionService.getTransactions();
        categories.value = await CategoryService.getCategories(); // Fetch categories
    } catch (error) {
        toast.add({ severity: 'error', summary: 'Error', detail: 'Failed to load data', life: 3000 });
    }
});

// Open the dialog for creating a new transaction
function openNew() {
    transaction.value = {
        categoryId: '',
        type: null,
        amount: 0,
        date: new Date().toISOString().split('T')[0],
        notes: '',
    };
    transactionDialog.value = true;
}

// Close the dialog
function hideDialog() {
    transactionDialog.value = false;
}

// Save the transaction
async function saveTransaction() {
    const payload = {
        Transaction: {
            CategoryId: transaction.value.categoryId,
            Type: transaction.value.type,
            Amount: transaction.value.amount,
            Date: transaction.value.date ? new Date(transaction.value.date).toISOString() : null,
            Notes: transaction.value.notes || null,
        },
    };

    console.log('Payload:', payload);

    try {
        const response = await TransactionService.createTransaction(payload);
        transactions.value.push(response);
        toast.add({ severity: 'success', summary: 'Success', detail: 'Transaction created', life: 3000 });
        transactionDialog.value = false;
    } catch (error) {
        console.error('Error creating transaction:', error.response?.data || error.message);

        // Log validation errors if present
        if (error.response?.data?.errors) {
            console.error('Validation Errors:', error.response.data.errors);
        }

        toast.add({
            severity: 'error',
            summary: 'Error',
            detail: error.response?.data?.title || 'Failed to create transaction',
            life: 3000,
        });
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
                        placeholder="Select a Category"
                    />
                </div>

                <div class="field">
                    <label for="type" class="block">Type</label>
                    <Dropdown
                        id="type"
                        v-model="transaction.type"
                        :options="typeOptions"
                        optionLabel="label"
                        optionValue="value"
                        placeholder="Select a Type"
                    />
                </div>

                <div class="field">
                    <label for="amount" class="block">Amount</label>
                    <InputNumber
                        id="amount"
                        v-model="transaction.amount"
                        mode="currency"
                        currency="USD"
                        locale="en-US"
                    />
                </div>

                <div class="field">
                    <label for="date" class="block">Date</label>
                    <InputText
                        id="date"
                        type="date"
                        v-model="transaction.date"
                    />
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
.field {
    margin-bottom: 1rem;
}
</style>
