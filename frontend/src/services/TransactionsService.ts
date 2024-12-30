import api from '@/services/Api';

export const TransactionService = {
  getTransactions: async () => {
    try {
      const response = await api.get('/Transactions');
      return response.data;
    } catch (error) {
      console.error('Error fetching transactions:', error);
      throw error;
    }
  },

  createTransaction: async (payload) => {
    try {
        const response = await api.post('/Transactions', payload); // payload now includes the Transaction wrapper
        return response.data;
    } catch (error) {
        console.error('Error creating transaction:', error.response?.data || error.message);
        throw error;
    }
},
};
