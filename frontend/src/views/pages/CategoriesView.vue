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
const types = [
  { label: "Income", value: "Income" },
  { label: "Expense", value: "Expense" }
];

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

  if (!category.value.name) {
    toast.add({ severity: 'error', summary: 'Error', detail: 'Name is required', life: 3000 });
    return;
  }

  const payload = {
    name: category.value.name,
    description: category.value.description || "",
    type: category.value.type.value === "Income" ? 1 : 2 // Map dropdown value to enum
  };

  if (category.value.id) {
    // Update category
    CategoryService.updateCategory({ ...payload, id: category.value.id })
      .then(() => {
        toast.add({ severity: 'success', summary: 'Successful', detail: 'Category Updated', life: 3000 });
        categoryDialog.value = false;
        fetchCategories();
      });
  } else {
    // Create category
    CategoryService.createCategory(payload)
      .then(() => {
        toast.add({ severity: 'success', summary: 'Successful', detail: 'Category Created', life: 3000 });
        categoryDialog.value = false;
        fetchCategories();
      });
  }
}

function editCategory(cat) {
  category.value = { ...cat };
  categoryDialog.value = true;
}

function confirmDeleteCategory(cat) {
  category.value = cat; // Save the category to be deleted
  deleteCategoryDialog.value = true; // Open the confirmation dialog
}

function deleteCategory(categoryId) {
  CategoryService.deleteCategory(categoryId)
    .then(() => {
      toast.add({ severity: 'success', summary: 'Successful', detail: 'Category Deleted', life: 3000 });
      categories.value = categories.value.filter((category) => category.id !== categoryId); // Remove from UI
      deleteCategoryDialog.value = false; // Close the confirmation dialog
    })
    .catch((error) => {
      console.error('Error deleting category:', error);
      toast.add({ severity: 'error', summary: 'Error', detail: 'Could not delete category', life: 3000 });
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

    <Dialog
      v-model:visible="categoryDialog"
      :style="{ width: '450px' }"
      header="Category Details"
      :modal="true"
    >
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
          <Select v-model="category.type" :options="types" optionLabel="label" placeholder="Select a Type" />
        </div>
      </div>

      <template #footer>
        <Button label="Cancel" icon="pi pi-times" text @click="hideDialog" />
        <Button label="Save" icon="pi pi-check" @click="saveCategory" />
      </template>
    </Dialog>

    <Dialog
      v-model:visible="deleteCategoryDialog"
      :style="{ width: '450px' }"
      header="Confirm"
      :modal="true"
    >
      <p>Are you sure you want to delete this category?</p>
      <template #footer>
        <Button label="No" icon="pi pi-times" text @click="deleteCategoryDialog = false" />
        <Button label="Yes" icon="pi pi-check" severity="danger" @click="deleteCategory(category.id)" />
      </template>
    </Dialog>
  </div>
</template>
