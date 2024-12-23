<template>
  <div>
    <CategoryList
      :categories="categories"
      @add="showAddForm = true"
      @edit="editCategory"
      @refresh="fetchCategories"
    />
    <AddCategory v-if="showAddForm" @refresh="handleRefresh" />
    <EditCategory
      v-if="selectedCategory"
      :category="selectedCategory"
      @refresh="handleRefresh"
    />
  </div>
</template>

<script>
import CategoryList from '@/components/TransactionCategory/CategoryList.vue';
import AddCategory from '@/components/TransactionCategory/AddCategory.vue';
import EditCategory from '@/components/TransactionCategory/EditCategory.vue';
import categoryService from '@/services/categoryService';

export default {
  components: { CategoryList, AddCategory, EditCategory },
  data() {
    return {
      categories: [],
      showAddForm: false,
      selectedCategory: null,
    };
  },
  methods: {
    async fetchCategories() {
      try {
        this.categories = await categoryService.getCategories();
        this.showAddForm = false;
        this.selectedCategory = null;
      } catch (error) {
        console.error('Error fetching categories:', error);
      }
    },
    editCategory(category) {
      this.selectedCategory = { ...category };
    },
    handleRefresh() {
      this.fetchCategories();
    },
  },
  created() {
    this.fetchCategories();
  },
};
</script>
