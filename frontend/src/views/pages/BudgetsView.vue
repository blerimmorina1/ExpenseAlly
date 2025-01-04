<script setup>
import { BudgetService } from '@/services/BudgetService';
import { FilterMatchMode } from '@primevue/core/api';
import { useToast } from 'primevue/usetoast';
import { onMounted, ref } from 'vue';
import { watch } from 'vue';

const selectedMonth = ref(new Date(new Date().getFullYear(), new Date().getMonth(), 1));
const toast = useToast();
const dt = ref();
const categoriesBudget = ref();
const categories = ref();
const totalLimit = ref();
const totalSpent = ref();
const budgetId = ref();
const budgetName = ref();
const totalCategoriesLimit = ref(0);
const categoryBudgetDialog = ref(false);
const budgetDialog = ref(false);
const deleteBudgetDialog = ref(false);
const categoryBudget = ref({});
const filters = ref({
    global: { value: null, matchMode: FilterMatchMode.CONTAINS }
});
const submitted = ref(false);
const categoryBudgetSubmitted = ref(false);

onMounted(() => {
    fetchBudgetData();
});

const mergeCategoriesWithBudget = (categories, categoriesBudget) => {
    if (!Array.isArray(categories.value)) {
        console.error('categories.value is not an array:', categories.value);
        return;
    }

    if (!Array.isArray(categoriesBudget.value)) {
        console.error('categoriesBudget is not an array:', categoriesBudget);
        return;
    }

    categories.value = categories.value.map((category) => {
        const matchingBudget = categoriesBudget.value.find(
            (budget) => budget.categoryId === category.categoryId
        );

        if (matchingBudget) {
            return {
                ...category,
                id: matchingBudget.id,
                limit: matchingBudget.limit,
            };
        }

        return { ...category };
    });
};

async function fetchBudgetData() {
    try {
        const data = await BudgetService.getBudget(selectedMonth.value);
        categoriesBudget.value = data.budgetDetails;
        totalLimit.value = data.totalLimit;
        totalSpent.value = data.totalSpent;
        budgetId.value = data.id;
        budgetName.value = data.name;
        totalCategoriesLimit.value = data.id ? data.totalLimit : 0;
    } catch (error) {
        console.error('Error fetching budgets:', error);
    }
}

async function fetchCategories() {
    try {
        const data = await BudgetService.getCategories();
        categories.value = data.categories;

        if (categoriesBudget.value) {
            mergeCategoriesWithBudget(categories, categoriesBudget);
        }
    } catch (error) {
        console.error('Error fetching categories:', error);
    }
}

function openNew() {
    categoryBudget.value = {};
    submitted.value = false;
    budgetDialog.value = true;
    fetchCategories()
}

function hideDialog() {
    budgetDialog.value = false;
    submitted.value = false;
}

async function saveBudget() {
    submitted.value = true;
    try {
        if (totalCategoriesLimit.value.limit <= 0) {
            toast.add({ severity: 'warn', summary: 'Warning', detail: 'Budget limit must be greater than 0.', life: 3000 });
            return;
        }

        var apiResponse = await BudgetService.saveBudget(budgetId.value, budgetName.value, selectedMonth.value, totalCategoriesLimit.value, categories.value);

        if (!apiResponse.data.success) {
            if (apiResponse.data.errors && Array.isArray(apiResponse.data.errors)) {
                const errorMessages = apiResponse.data.errors
                    .map(error => error.message)
                    .join('\n');
                toast.add({
                    severity: 'error',
                    summary: 'Error',
                    detail: errorMessages,
                    life: 3000
                });
            } else {
                toast.add({
                    severity: 'error',
                    summary: 'Error',
                    detail: 'An unexpected error occurred.',
                    life: 3000
                });
            }
            return;
        }

        toast.add({
            severity: 'success',
            summary: 'Success',
            detail: `Budget saved successfully.`,
            life: 3000
        });

        budgetDialog.value = false;
        fetchBudgetData();
    } catch (error) {
        console.error('Error saving budget:', error);
        toast.add({ severity: 'error', summary: 'Error', detail: `Failed to save the budget.`, life: 3000 });
    }
}

function hideCategoryBudgetDialog() {
    categoryBudgetDialog.value = false;
    categoryBudgetSubmitted.value = false;
}

function editCategoryBudget(cat) {
    categoryBudget.value = { ...cat };
    categoryBudgetDialog.value = true;
}

async function saveCategoryBudget() {
    categoryBudgetSubmitted.value = true;
    try {
        if (categoryBudget.value.limit <= 0) {
            toast.add({ severity: 'warn', summary: 'Warning', detail: 'Budget limit must be greater than 0.', life: 3000 });
            return;
        }

        var apiResponse = await BudgetService.saveCategoryBudget(categoryBudget.value);

        if (!apiResponse.data.success) {
            if (apiResponse.data.errors && Array.isArray(apiResponse.data.errors)) {
                const errorMessages = apiResponse.data.errors
                    .map(error => error.message)
                    .join('\n');
                toast.add({
                    severity: 'error',
                    summary: 'Error',
                    detail: errorMessages,
                    life: 3000
                });
            } else {
                toast.add({
                    severity: 'error',
                    summary: 'Error',
                    detail: 'An unexpected error occurred.',
                    life: 3000
                });
            }
            return;
        }

        toast.add({
            severity: 'success',
            summary: 'Success',
            detail: `Budget for ${categoryBudget.value.categoryName} saved successfully.`,
            life: 3000
        });

        categoryBudgetDialog.value = false;
        fetchBudgetData();
    } catch (error) {
        console.error('Error saving budget:', error);
        toast.add({ severity: 'error', summary: 'Error', detail: `Failed to save the budget for ${categoryBudget.value.categoryName}.`, life: 3000 });
    }
}

function confirmDeleteBudget() {
    deleteBudgetDialog.value = true;
}

async function deleteBudget() {
    try {
        if (!budgetId.value) {
            toast.add({ severity: 'warn', summary: 'Warning', detail: 'No budget selected for deletion.', life: 3000 });
            return;
        }

        await BudgetService.deleteBudget(budgetId.value);

        toast.add({ severity: 'success', summary: 'Success', detail: 'Budget deleted successfully.', life: 3000 });
        deleteBudgetDialog.value = false;
        fetchBudgetData();
    } catch (error) {
        console.error('Error deleting budget:', error);
        toast.add({ severity: 'error', summary: 'Error', detail: 'Failed to delete the budget.', life: 3000 });
    }
}

function exportCSV() {
    dt.value.exportCSV();
}

function calculateProgress(spent, limit) {
    if (limit === 0) {
        return { percentage: 0, overspent: false, overspentAmount: 0 };
    }
    if (spent === 0) {
        return { percentage: 0, overspent: false, overspentAmount: 0 };
    }
    const percentage = Math.min((spent / limit) * 100, 100).toFixed(2);
    const overspent = spent > limit;
    const overspentAmount = overspent ? (spent - limit).toFixed(2) : 0; // Calculate overspent amount
    return {
        percentage: parseFloat(percentage),
        overspent,
        overspentAmount: parseFloat(overspentAmount)
    };
}

function navigateMonth(direction) {
    const currentMonth = this.selectedMonth;
    // Create a new date by adding or subtracting a month
    const newMonth = new Date(
        currentMonth.getFullYear(),
        currentMonth.getMonth() + direction,
        1
    );
    this.selectedMonth = newMonth;

    fetchBudgetData(this.selectedMonth.value);
}

function calculateTotalLimit() {
      let total = 0;
      categories.value.forEach(category => {
        total += category.limit || 0;
      });
      totalCategoriesLimit.value = total;
    }

    // Watch for changes in categories to trigger total limit update
    watch(categories, calculateTotalLimit, { deep: true });
</script>

<style>
.p-progressbar-danger .p-progressbar-value {
    background-color: red !important;
}
</style>

<template>
    <div>
        <div class="card">
            <span class="text-primary">
                {{ budgetName }}
            </span>
            <div class="w-full text-center mb-4">
                <div class="inline-block">
                    <label for="month-picker" class="font-bold block mb-2">Month</label>
                    <div class="flex items-center justify-center">
                        <!-- Left arrow button -->
                        <button class="p-2 text-white bg-gray-300 hover:bg-gray-500 rounded-l-md"
                            @click="navigateMonth(-1)">
                            &lt;
                        </button>

                        <!-- Month picker -->
                        <DatePicker id="month-picker" v-model="selectedMonth" view="month" dateFormat="MM yy"
                            class="w-full mx-2" change="fetchBudgetData()" />

                        <!-- Right arrow button -->
                        <button class="p-2 text-white bg-gray-300 hover:bg-gray-500 rounded-r-md"
                            @click="navigateMonth(1)">
                            &gt;
                        </button>
                    </div>
                </div>
            </div>
            <Toolbar class="mb-6">
                <template #start>
                    <div class="flex flex-wrap gap-4 items-center justify-center">
                        <!-- Total Limit -->
                        <div v-if="budgetId" class="text-center bg-gray-100 p-4 rounded-lg shadow-md">
                            <h4 class="text-lg font-bold text-gray-700 mb-2">Total Limit</h4>
                            <span class="text-xl font-semibold text-green-600">
                                {{ $formatters.formatCurrency(totalLimit) }}
                            </span>
                        </div>

                        <!-- Total Spent -->
                        <div v-if="budgetId" class="text-center bg-gray-100 p-4 rounded-lg shadow-md">
                            <h4 class="text-lg font-bold text-gray-700 mb-2">Total Spent</h4>
                            <span class="text-xl font-semibold text-red-600">
                                {{ $formatters.formatCurrency(totalSpent) }}
                            </span>
                        </div>
                    </div>
                </template>

                <template #end>
                    <!-- New/Edit Button -->
                    <Button :label="budgetId ? 'Edit' : 'New'" :icon="budgetId ? 'pi pi-pencil' : 'pi pi-plus'"
                        severity="secondary" class="mr-2" @click="openNew()" />
                    <!-- Export Button -->
                    <div v-if="budgetId">
                        <Button label="Export" icon="pi pi-upload" severity="secondary" @click="exportCSV($event)" />
                        <Button icon="pi pi-trash" class="ml-1" outlined rounded severity="danger"
                            @click="confirmDeleteBudget()" />
                    </div>
                </template>
            </Toolbar>
            <DataTable ref="dt" :value="categoriesBudget" dataKey="id" :paginator="true" :rows="10" :filters="filters"
                paginatorTemplate="FirstPageLink PrevPageLink PageLinks NextPageLink LastPageLink CurrentPageReport RowsPerPageDropdown"
                :rowsPerPageOptions="[5, 10, 25]"
                currentPageReportTemplate="Showing {first} to {last} of {totalRecords} categories">
                <template #header>
                    <div class="flex flex-wrap gap-2 items-center justify-between">
                        <h4 class="m-0">Budget for categories</h4>
                        <IconField>
                            <InputIcon>
                                <i class="pi pi-search" />
                            </InputIcon>
                            <InputText v-model="filters['global'].value" placeholder="Search..." />
                        </IconField>
                    </div>
                </template>

                <Column field="categoryName" header="Category" sortable style="min-width: 12rem">
                    <template #body="slotProps">
                        {{ slotProps.data.categoryName }}
                    </template>
                </Column>
                <Column field="limit" header="Limit" sortable style="min-width: 8rem">
                    <template #body="slotProps">
                        {{ $formatters.formatCurrency(slotProps.data.limit) }}
                    </template>
                </Column>

                <Column field="spent" header="Spent" sortable style="min-width: 8rem">
                    <template #body="slotProps">
                        {{ $formatters.formatCurrency(slotProps.data.spent) }}
                    </template>
                </Column>
                <Column :exportable="false" header="" style="min-width: 12rem">
                    <template #body="slotProps">
                        <div>
                            <!-- Progress Bar -->
                            <ProgressBar
                                :value="calculateProgress(slotProps.data.spent, slotProps.data.limit).percentage"
                                :class="[
                                    'w-full h-3',
                                    calculateProgress(slotProps.data.spent, slotProps.data.limit).overspent ? 'p-progressbar-danger' : ''
                                ]" />

                            <!-- Overspent Message -->
                            <span v-if="calculateProgress(slotProps.data.spent, slotProps.data.limit).overspent"
                                class="text-red-500 text-sm">
                                Overspent the budget by
                                {{ $formatters.formatCurrency(calculateProgress(slotProps.data.spent,
                                    slotProps.data.limit).overspentAmount) }}
                            </span>
                        </div>
                    </template>
                </Column>


                <Column :exportable="false" style="min-width: 12rem">
                    <template #body="slotProps">
                        <Button icon="pi pi-eye" outlined rounded severity="info" class="mr-2"
                            @click="viewTransactions(slotProps.data)" v-tooltip="'View Transactions'" />
                        <Button icon="pi pi-pencil" outlined rounded class="mr-2"
                            @click="editCategoryBudget(slotProps.data)" v-tooltip="'Edit Budget'" />
                    </template>
                </Column>

            </DataTable>
        </div>

        <Dialog v-model:visible="budgetDialog" :style="{ width: '450px' }"
            :header="budgetId ? 'Edit Budget' : 'Create Budget'" :modal="true">
            <div class="flex flex-col gap-6">
                <!-- Budget Name -->
                <div>
                    <label for="name" class="block font-bold mb-3">Name</label>
                    <InputText id="name" v-model.trim="budgetName" required="true" autofocus
                        :invalid="submitted && !budgetName" fluid />
                    <small v-if="submitted && !budgetName" class="text-red-500">Name is required.</small>
                </div>

                <!-- Categories with Limits -->
                <div>
                    <h4 class="font-bold mb-3">Categories</h4>
                    <div class="grid grid-cols-12 gap-4" style="max-height: 200px; overflow-y: scroll;">
                        <div class="col-span-12 flex items-center justify-between mb-2"
                            v-for="(category, index) in categories" :key="category.categoryId">
                            <span>{{ category.categoryName }}</span>
                            <InputNumber v-model="category.limit" mode="currency" currency="EUR" locale="de-DE"
                                class="mr-2" placeholder="Set limit" @input="calculateTotalLimit" />
                        </div>
                    </div>
                </div>

                <!-- Total Limit -->
                <div class="grid grid-cols-12 gap-4">
                    <div class="col-span-6">
                        <label for="total-limit" class="block font-bold mb-3">Total Limit</label>
                        <InputNumber id="total-limit" v-model="totalCategoriesLimit" mode="currency" currency="EUR"
                            locale="de-DE" readonly fluid />
                    </div>
                </div>
            </div>

            <!-- Footer -->
            <template #footer>
                <Button label="Cancel" icon="pi pi-times" text @click="hideDialog" />
                <Button label="Save" icon="pi pi-check" @click="saveBudget" />
            </template>
        </Dialog>

        <Dialog v-model:visible="deleteBudgetDialog" :style="{ width: '450px' }" header="Confirm" :modal="true">
            <div class="flex items-center gap-4">
                <i class="pi pi-exclamation-triangle !text-3xl" />
                <span v-if="budgetId">Are you sure you want to delete <b>{{ budgetName
                        }}</b>?</span>
            </div>
            <template #footer>
                <Button label="No" icon="pi pi-times" text @click="deleteBudgetDialog = false" />
                <Button label="Yes" icon="pi pi-check" @click="deleteBudget" />
            </template>
        </Dialog>

        <Dialog v-model:visible="categoryBudgetDialog" :style="{ width: '450px' }" header="Edit Category Budget"
            :modal="true">
            <div class="flex flex-col gap-6">
                <div class="col-span-12 flex items-center justify-between mb-2">
                    <span>{{ categoryBudget.categoryName }}</span>
                    <InputNumber v-model="categoryBudget.limit" mode="currency" currency="EUR" locale="de-DE"
                        placeholder="Set limit" />
                </div>
            </div>
            <template #footer>
                <Button label="Cancel" icon="pi pi-times" text @click="hideCategoryBudgetDialog" />
                <Button label="Save" icon="pi pi-check" @click="saveCategoryBudget" />
            </template>
        </Dialog>
    </div>
</template>
