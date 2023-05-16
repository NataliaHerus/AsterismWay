import { Category } from "./category";
import { Frequency } from "./frequency";

export interface Event {
    id?: number;
    name?: string;
    description?: string;
    startDate?: Date;
    endDate?: Date;
    year?: number;
    frequency?: Frequency;
    category?: Category
}
