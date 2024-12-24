import api from '@/services/Api';

export const CategoryService = {
  getCategories: async () => {
    try {
      const response = await api.get('/Categories');
      return response.data;
    } catch (error) {
      console.error('Error fetching categories:', error);
      throw error;
    }
  },

  createCategory: async (category) => {
    try {
      const response = await api.post('/Categories', category);
      return response.data;
    } catch (error) {
      console.error('Error creating category:', error);
      throw error;
    }
  },

  updateCategory: async (category) => {
    try {
      const response = await api.put(`/Categories/${category.id}`, category);
      return response.data;
    } catch (error) {
      console.error('Error updating category:', error);
      throw error;
    }
  },

  deleteCategory: async (id) => {
    try {
      await api.delete(`/Categories/${id}`);
    } catch (error) {
      console.error('Error deleting category:', error);
      throw error;
    }
  },

  deleteMultipleCategories: async (ids) => {
    try {
      await Promise.all(ids.map((id) => api.delete(`/Categories/${id}`)));
    } catch (error) {
      console.error('Error deleting multiple categories:', error);
      throw error;
    }
  },
};
