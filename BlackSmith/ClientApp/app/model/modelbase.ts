export class ModelBase {
    id: number;
    createdOn: Date;
    createdBy: number;
    modifiedOn: Date;
    modifiedBy: number;
    msg: string;
    isFailure: boolean = false;
}