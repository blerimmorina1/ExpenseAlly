<script setup>
import { CategoryService } from '@/services/CategoryService';
import { FilterMatchMode } from '@primevue/core/api';
import { useToast } from 'primevue/usetoast';
import { onMounted, ref } from 'vue';

const categories = ref([]);
const selectedCategories = ref();
const categoryDialog = ref(false);
const deleteCategoryDialog = ref(false);
const deleteCategoriesDialog = ref(false);
const category = ref({});
const submitted = ref(false);
const dt = ref();
const filters = ref({
  global: { value: null, matchMode: FilterMatchMode.CONTAINS },
});

const toast = useToast();

onMounted(() => {
  fetchCategories();
});

async function fetchCategories() {
  try {
    const data = await CategoryService.getCategories();
    categories.value = data;
  } catch (error) {
    console.error('Error fetching categories:', error);
  }
}

function openNew() {
  category.value = {};
  submitted.value = false;
  categoryDialog.value = true;
}

function hideDialog() {
  categoryDialog.value = false;
  submitted.value = false;
}

function saveCategory() {
  submitted.value = true;

  if (category.value.name?.trim()) {
    if (category.value.id) {
      CategoryService.updateCategory(category.value).then(() => {
        categories.value = categories.value.map((c) =>
          c.id === category.value.id ? category.value : c
        );
        toast.add({ severity: 'success', summary: 'Successful', detail: 'Category Updated', life: 3000 });
        hideDialog();
      });
    } else {
      CategoryService.createCategory(category.value).then((newCategory) => {
        categories.value.push(newCategory);
        toast.add({ severity: 'success', summary: 'Successful', detail: 'Category Created', life: 3000 });
        hideDialog();
      });
    }
  }
}

function editCategory(cat) {
  category.value = { ...cat };
  categoryDialog.value = true;
}

function confirmDeleteCategory(cat) {
  category.value = cat;
  deleteCategoryDialog.value = true;
}

function deleteCategory() {
  CategoryService.deleteCategory(category.value.id).then(() => {
    categories.value = categories.value.filter((val) => val.id !== category.value.id);
    toast.add({ severity: 'success', summary: 'Successful', detail: 'Category Deleted', life: 3000 });
    deleteCategoryDialog.value = false;
    category.value = {};
  });
}

function confirmDeleteSelected() {
  deleteCategoriesDialog.value = true;
}

function deleteSelectedCategories() {
  const ids = selectedCategories.value.map((c) => c.id);
  CategoryService.deleteMultipleCategories(ids).then(() => {
    categories.value = categories.value.filter((val) => !selectedCategories.value.includes(val));
    toast.add({ severity: 'success', summary: 'Successful', detail: 'Categories Deleted', life: 3000 });
    deleteCategoriesDialog.value = false;
    selectedCategories.value = null;
  });
}

function exportCSV() {
  dt.value.exportCSV();
}
</script>

<template>
  <div>
    <div class="card">
      <Toolbar class="mb-6">
        <template #start>
          <Button label="New" icon="pi pi-plus" severity="secondary" class="mr-2" @click="openNew" />
          <Button label="Delete" icon="pi pi-trash" severity="secondary" @click="confirmDeleteSelected" :disabled="!selectedCategories || !selectedCategories.length" />
        </template>
        <template #end>
          <Button label="Export" icon="pi pi-upload" severity="secondary" @click="exportCSV" />
        </template>
      </Toolbar>

      <DataTable
        ref="dt"
        v-model:selection="selectedCategories"
        :value="categories"
        dataKey="id"
        :paginator="true"
        :rows="10"
        :filters="filters"
        paginatorTemplate="FirstPageLink PrevPageLink PageLinks NextPageLink LastPageLink CurrentPageReport RowsPerPageDropdown"
        :rowsPerPageOptions="[5, 10, 25]"
        currentPageReportTemplate="Showing {first} to {last} of {totalRecords} categories"
      >
        <template #header>
          <div class="flex justify-between items-center">
            <h4 class="m-0">Manage Categories</h4>
            <InputText v-model="filters.global.value" placeholder="Search..." />
          </div>
        </template>

        <Column selectionMode="multiple" style="width: 3rem" :exportable="false"></Column>
        <Column field="name" header="Name" sortable style="min-width: 16rem"></Column>
        <Column field="description" header="Description" sortable style="min-width: 20rem"></Column>
        <Column field="type" header="Type" sortable style="min-width: 10rem"></Column>
        <Column :exportable="false" style="min-width: 12rem">
          <template #body="slotProps">
            <Button icon="pi pi-pencil" outlined rounded class="mr-2" @click="editCategory(slotProps.data)" />
            <Button icon="pi pi-trash" outlined rounded severity="danger" @click="confirmDeleteCategory(slotProps.data)" />
          </template>
        </Column>
      </DataTable>
    </div>

    <Dialog v-model:visible="categoryDialog" :style="{ width: '450px' }" header="Category Details" :modal="true">
      <div>
        <div>
          <label for="name" class="block font-bold mb-3">Name</label>
          <InputText id="name" v-model="category.name" required autofocus />
          <small v-if="submitted && !category.name" class="p-error">Name is required.</small>
        </div>
        <div>
          <label for="description" class="block font-bold mb-3">Description</label>
          <Textarea id="description" v-model="category.description" rows="3" cols="20" />
        </div>
        <div>
          <label for="type" class="block font-bold mb-3">Type</label>
          <Select v-model="category.type" :options="[{ label: 'Income', value: 'Income' }, { label: 'Expense', value: 'Expense' }]" placeholder="Select a Type" />
        </div>
      </div>

      <template #footer>
        <Button label="Cancel" icon="pi pi-times" text @click="hideDialog" />
        <Button label="Save" icon="pi pi-check" @click="saveCategory" />
      </template>
    </Dialog>
  </div>
</template>
