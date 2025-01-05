<script setup lang="ts">
import { ref, onMounted } from 'vue';
import { useToast } from 'primevue/usetoast';
import { TransactionService } from '@/services/TransactionsService';
import { CategoryService } from '@/services/CategoryService';

const transactions = ref([]);
const categories = ref([]);
const transactionDialog = ref(false);
const transaction = ref({
    categoryId: '',
    type: null,
    amount: 0,
    date: new Date().toISOString().split('T')[0],
    notes: '',
});
const toast = useToast();
const typeOptions = [
    { label: 'Income', value: 1 },
    { label: 'Expense', value: 2 },
];

onMounted(async () => {
    try {

        const fetchedCategories = await CategoryService.getCategories();
        categories.value = fetchedCategories.map(category => ({
            id: category.id,
            name: category.name,
        }));

        console.log('Fetched Categories:', categories.value);

        await fetchTransactions();
    } catch (error) {
        console.error('Error fetching data:', error);
        toast.add({ severity: 'error', summary: 'Error', detail: 'Failed to load data', life: 3000 });
    }
});

async function fetchTransactions() {
    try {
        const fetchedTransactions = await TransactionService.getTransactions();
        transactions.value = fetchedTransactions.map(transaction => ({
            id: transaction.id,
            amount: transaction.amount || 0,
            date: transaction.date || null,
            notes: transaction.notes || '',
            type: transaction.type,
            categoryName: transaction.category?.name || 'Unknown',
        }));
        console.log('Fetched Transactions:', transactions.value);
    } catch (error) {
        console.error('Error fetching transactions:', error);
        toast.add({ severity: 'error', summary: 'Error', detail: 'Failed to load transactions', life: 3000 });
    }
}

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

function hideDialog() {
    transactionDialog.value = false;
}

async function saveTransaction() {
    if (!transaction.value.categoryId || !transaction.value.type || transaction.value.amount <= 0) {
        toast.add({ severity: 'error', summary: 'Error', detail: 'Please fill all required fields', life: 3000 });
        return;
    }

    const payload = {
        Transaction: {
            CategoryId: transaction.value.categoryId,
            Type: transaction.value.type,
            Amount: transaction.value.amount,
            Date: transaction.value.date ? new Date(transaction.value.date).toISOString() : null,
            Notes: transaction.value.notes?.trim() || null,
        },
    };

    try {
        await TransactionService.createTransaction(payload);

        await fetchTransactions();

        toast.add({ severity: 'success', summary: 'Success', detail: 'Transaction created and fetched', life: 3000 });


        transactionDialog.value = false;
    } catch (error) {
        console.error('Error creating transaction:', error.response?.data || error.message);
        toast.add({
            severity: 'error',
            summary: 'Error',
            detail: error.response?.data?.title || 'Failed to create transaction',
            life: 3000,
        });
    }
}

const firstRowIndex = ref(0);

function resetPagination(event) {
    firstRowIndex.value = event.first;
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

          <DataTable :value="transactions" :paginator="true" :rows="10" :first="firstRowIndex" @update:page="resetPagination">
              <Column field="categoryName" header="Category" sortable></Column>
              <Column field="type" header="Type" sortable>
                  <template #body="slotProps">
                      {{ slotProps.data.type === 1 ? 'Income' : 'Expense' }}
                  </template>
              </Column>
              <Column field="amount" header="Amount" sortable></Column>
              <Column field="date" header="Date" sortable>
                  <template #body="slotProps">
                      {{ slotProps.data.date ? new Date(slotProps.data.date).toLocaleDateString() : 'Invalid Date' }}
                  </template>
              </Column>
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
