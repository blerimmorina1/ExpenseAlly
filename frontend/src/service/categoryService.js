import apiClient from '@/services/api';

const categoryService = {
  getCategories: async () => {
    try {
      const response = await apiClient.get('/Categories');
      return response.data;
    } catch (error) {
      console.error('Error fetching categories:', error);
      throw error;
    }
  },


  createCategory: async (category) => {
    try {
      const response = await apiClient.post('/Categories', category);
      return response.data;
    } catch (error) {
      console.error('Error creating category:', error);
      throw error;
    }
  },

  updateCategory: async (category) => {
    try {
      await apiClient.put('/Categories', category);
    } catch (error) {
      console.error('Error updating category:', error);
      throw error;
    }
  },

  deleteCategory: async (id) => {
    try {
      await apiClient.delete(`/Categories/${id}`);
    } catch (error) {
      console.error('Error deleting category:', error);
      throw error;
    }
  },
};

export default categoryService;
