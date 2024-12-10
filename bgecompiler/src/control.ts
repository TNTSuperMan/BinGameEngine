import genid from "./genid";
import { not } from "./bool";
import { Expr } from "./native";
export type Exprs = string[]
export const If = (condition:Expr, trueCode: Exprs, falseCode: Exprs = [], comment?: string):Expr => {
    let ret = "";
    const id = (comment ? `_${comment}` : "") + genid();
    const trueTag = ":if_true"+id;
    const endTag = ":if_end"+id;

    ret += condition + "\n";

    ret += `/ ${trueTag} truejump\n`
    ret += falseCode.join("");
    ret += `/ ${endTag} jump\n`;

    ret += `${trueTag}\n`;
    ret += trueCode.join("");
    ret += `${endTag}\n`;

    return ret;
}
export const While = (condition: Expr, code: Exprs, comment?: string):Expr => {
    let ret = "";
    const id = (comment ? `_${comment}` : "") + genid();
    const loopTag = ":while_loop" + id;
    const endTag = ":while_end" + id;
    ret += loopTag + "\n";
    ret += not(condition) + "/ "+endTag+" truejump\n";
    
    ret += code.join("")

    ret += "/ "+loopTag+" jump\n"
    ret += endTag + "\n";
    return ret;
}