import { ModelBase } from "./modelbase";

export class User extends ModelBase {
    name: string;
    userId: string;
    password: string;
}