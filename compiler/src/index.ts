import { fndefines } from "./fn.ts"
import { Expr } from "./native.ts"

export * from "./bool.ts"
export * from "./control.ts"
export * from "./fn.ts"
export * from "./native.ts"
export * from "./var.ts"
export const Bundle = (entry: Expr) => entry + fndefines;