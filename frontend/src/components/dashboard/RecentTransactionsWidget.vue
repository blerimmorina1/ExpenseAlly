<script setup lang="ts">
import { TransactionService } from '@/services/TransactionsService';
import { onMounted, ref } from 'vue';

const transactions = ref(null);

onMounted(() => {
  TransactionService.getTransactions().then((data) => (transactions.value = data));
});
function getCategoryName(categoryId: string): string {
  const category = categories.value.find((cat: any) => cat.id === categoryId);
  return category ? category.name : 'Unknown';
}
</script>

<template>
    <div class="card">
        <div class="font-semibold text-xl mb-4">Recent Transactions</div>
        <DataTable :value="transactions" :rows="5" :paginator="true" responsiveLayout="scroll">
<!--            <Column field="categoryId" header="Category" sortable>-->
<!--                <template #body="slotProps">-->
<!--                  {{ getCategoryName(slotProps.data.categoryId) }}-->
<!--                </template>-->
<!--            </Column>-->
            <Column field="type" header="Type" sortable>
                <template #body="slotProps">
                  {{ slotProps.data.type === 1 ? 'Income' : 'Expense' }}
                </template>
            </Column>
            <Column field="amount" header="Amount" sortable>
                <template #body="slotProps">
                  {{ $formatters.formatCurrency(slotProps.data.amount) }}
                </template>
            </Column>
            <Column field="date" header="Date" sortable>
              <template #body="slotProps">
                {{  $formatters.formatDate(slotProps.data.date) }}
              </template>
            </Column>
          <Column style="width: 15%" header="View">
            <template #body>
              <Button icon="pi pi-search" type="button" class="p-button-text"></Button>
            </template>
          </Column>
        </DataTable>
    </div>
</template>
