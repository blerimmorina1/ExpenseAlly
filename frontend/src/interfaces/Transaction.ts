export interface Transaction {
  id?: string;
  categoryId: string;
  type: number;
  name: string; // Add this field
  amount: number;
  date: string;
  notes?: string;
}
