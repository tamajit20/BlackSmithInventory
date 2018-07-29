import { ModelBase } from "./modelbase";

export class Product extends ModelBase {
    name: string;
    description: string;
  //  image: FileList;
    price: number;
}