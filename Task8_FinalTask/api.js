import axios from 'axios';

const API = 'https://localhost:7270/api';

export const getCategories = () => axios.get(`${API}/ExpenseCategories`);
export const getCategoryById = id => axios.get(`${API}/ExpenseCategories/${id}`);
export const createCategory = category => axios.post(`${API}/ExpenseCategories`, category);
export const updateCategory = (id, category) => axios.put(`${API}/ExpenseCategories/${id}`, category);
export const deleteCategory = id => axios.delete(`${API}/ExpenseCategories/${id}`);

export const getItems = () => axios.get(`${API}/ExpenseItems`);
export const getItemById = id => axios.get(`${API}/ExpenseItems/${id}`);
export const createItem = item => axios.post(`${API}/ExpenseItems`, item);
export const updateItem = (id, item) => axios.put(`${API}/ExpenseItems/${id}`, item);
export const deleteItem = id => axios.delete(`${API}/ExpenseItems/${id}`);

export const getTransactions = () => axios.get(`${API}/ExpenseTransactions`);
export const getTransactionById = id => axios.get(`${API}/ExpenseTransactions/${id}`);
export const createTransaction = transaction => axios.post(`${API}/ExpenseTransactions`, transaction);
export const updateTransaction = (id, transaction) => axios.put(`${API}/ExpenseTransactions/${id}`, transaction);
export const deleteTransaction = id => axios.delete(`${API}/ExpenseTransactions/${id}`);
export const getTransactionsByDay = date => axios.get(`${API}/ExpenseTransactions/byday/${date}`);
export const getTransactionsByMonth = (year, month) => axios.get(`${API}/ExpenseTransactions/bymonth/${year}/${month}`);
export const getDaySticker = date => axios.get(`${API}/ExpenseTransactions/sticker/${date}`);