import { Category } from "./category";

export interface SummaryItem
{
    items: Category[];
    total_count: number;
}