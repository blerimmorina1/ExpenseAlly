export interface Category {
  id?: string; // Optional for new categories
  name: string;
  description?: string;
  type: 1 | 2; // Representing 'Income' as 1 and 'Expense' as 2
}
