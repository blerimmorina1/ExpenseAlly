import api from '@/services/Api'

export const BudgetService = {
  getBudget: async (date) => {
    try {
      const response = await api.get('/budgets/getBudget', {
        params: { date },
      })
      return response.data
    } catch (error) {
      throw error
    }
  },

  getCategories: async () => {
    try {
      const response = await api.get('/budgets/getExpenseCategories')
      return response.data
    } catch (error) {
      throw error
    }
  },

  deleteBudget: async (id) => {
    try {
      await api.delete('/budgets/deleteBudget', {
        params: { id },
      })
    } catch (error) {
      console.error('Error deleting budget:', error)
      throw error
    }
  },

  saveCategoryBudget: async (categoryBudget) => {
    try {
      var apiResponse = await api.put('/budgets/saveCategoryBudget', categoryBudget)
      return apiResponse
    } catch (error) {
      console.error('Error saving budget:', error)
      throw error
    }
  },

  saveBudget: async (
    budgetId,
    budgetName,
    selectedMonth,
    totalCategoriesLimit,
    categoriesBudget,
  ) => {
    try {
      debugger;
      const apiEndpoint = budgetId ? 'editBudget' : 'createBudget';
      const filteredCategoriesBudget = categoriesBudget.filter((category) => category.limit > 0);
      
      const body = {
        id: budgetId,
        name: budgetName,
        startDate: selectedMonth,
        totalLimit: totalCategoriesLimit,
        budgetDetails: filteredCategoriesBudget,
      };
  
      const apiResponse = await api.post(`/budgets/${apiEndpoint}`, body); 
      return apiResponse;
    } catch (error) {
      console.error('Error saving budget:', error);
      throw error; 
    }
  },
  
}
