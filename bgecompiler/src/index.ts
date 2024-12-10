import { Exprs } from "./control"
import { bindefines, fndefines, resdefines } from "./fn"

export * from "./bool"
export * from "./control"
export * from "./fn"
export * from "./native"
export * from "./shortutil";
export * from "./var"
export const Bundle = (entry: Exprs) => entry.join("") + fndefines + resdefines + bindefines;