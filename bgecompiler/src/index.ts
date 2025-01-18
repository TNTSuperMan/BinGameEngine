import { Exprs } from "./control"
import genid from "./genid"
import { bindefines, fndefines, resdefines } from "./fn"

export * from "./bit"
export * from "./bool"
export * from "./control"
export * from "./fn"
export { genid }
export * from "./native"
export * from "./shortutil"
export * from "./var"
export const Bundle = (entry: Exprs) => entry.join("") + fndefines + resdefines + bindefines;