import { Exprs } from "./control.ts"
import { bindefines, fndefines } from "./fn.ts"
import { echoVars } from "./var.ts"

export * from "./bool.ts"
export * from "./control.ts"
export * from "./fn.ts"
export * from "./native.ts"
export * from "./shortutil.ts";
export * from "./var.ts"
export const Bundle = (entry: Exprs) => {
    echoVars();
    return entry.join("") + fndefines + "\n" + bindefines;
}