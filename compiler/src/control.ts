import genid from "genid.ts";
import { not } from "./bool.ts";
import { Expr } from "./native.ts";
export type Exprs = string[]
export const If = (condition:Expr, trueCode: Exprs, falseCode: Exprs = []):Expr => {
    let ret = condition + "\n";
    const id = genid();
    const trueTag = ":if_true"+id;
    const endTag = ":if_end"+id;

    ret += `/ ${trueTag} truejump\n`
    ret += falseCode.join("\n");
    ret += `\n/ ${endTag} jump\n`;

    ret += `\n${trueTag}\n`;
    ret += trueCode.join("\n");
    ret += `\n${endTag}\n`;

    return ret;
}
export const While = (condition: Expr, code: Exprs):Expr => {
    let ret = "";
    const id = genid();
    const loopTag = ":while_loop" + id;
    const endTag = ":while_end" + id;
    ret += loopTag + "\n";
    ret += not(condition) + "/ "+endTag+" truejump\n";
    
    ret += code.join("\n")

    ret += "/ "+loopTag+" jump\n"
    ret += endTag + "\n";
    return ret;
}