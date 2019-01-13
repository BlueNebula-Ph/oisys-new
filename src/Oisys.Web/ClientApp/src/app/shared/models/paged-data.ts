import { Page } from "./page";

/**
 * An array of data with an associated page object used for paging
 */
export class PagedData<T> {
    items = new Array<T>();
    pageInfo = new Page();
}
