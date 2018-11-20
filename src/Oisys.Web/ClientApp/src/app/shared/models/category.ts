export class Category {
    constructor(public id?: number, public name?: string, public rowVersion?: string) {
        id = id || 0;
        name = name || '';
        rowVersion = rowVersion || '';
    };
}