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

  createTransaction: async (transaction: { name: string; amount: number }) => {
    try {
      const response = await api.post('/Transactions', transaction);
      return response.data;
    } catch (error) {
      console.error('Error creating transaction:', error);
      throw error;
    }
  },
};
