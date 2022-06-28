import { Expense } from "./Expense";

export interface User{
    username: string;
    token: string;
    expenses: Expense[];
}

