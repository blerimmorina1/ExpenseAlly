import api from '@/services/ApiService';

export const SavingGoalService = {
  getSavingGoals: async () => {
    try {
      const response = await api.get('/SavingGoals');
      return response.data;
    } catch (error) {
      console.error('Error fetching saving goals:', error);
      throw error;
    }
  },

  createSavingGoal: async (savingGoal) => {
    try {
      const response = await api.post('/SavingGoals', savingGoal);
      return response.data;
    } catch (error) {
      console.error('Error creating saving goals:', error);
      throw error;
    }
  },

  updateSavingGoal: async (savingGoal) => {
    try {
      const response = await api.put(`/SavingGoals/${savingGoal.id}`, savingGoal);
      return response.data;
    } catch (error) {
      console.error('Error updating saving goals:', error);
      throw error;
    }
  },

  deleteSavingGoal: async (id) => {
    try {
      const response = await api.delete(`/SavingGoals/${id}`);
      return response.data;
    } catch (error) {
      console.error('Error deleting saving goals:', error);
      throw error;
    }
  }
};
