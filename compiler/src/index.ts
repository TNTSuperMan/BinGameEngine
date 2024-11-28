import { Exprs } from "./control.ts"
import { fndefines } from "./fn.ts"

export * from "./bool.ts"
export * from "./control.ts"
export * from "./fn.ts"
export * from "./native.ts"
export * as Sutil from "./shortutil.ts"
export * from "./var.ts"
export const Bundle = (entry: Exprs) => entry.join("\n") + fndefines;