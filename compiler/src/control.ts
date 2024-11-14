import { not } from "./bool.ts";
import { Expr } from "./native.ts";
type Exprs = string[]
export const If = (condition:Expr, trueCode: Exprs, falseCode: Exprs = []):Expr => {
    let ret = condition + "\n";
    const trueTag = ":"+crypto.randomUUID();
    const endTag = ":"+crypto.randomUUID();

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
    const loopTag = ":" + crypto.randomUUID();
    const endTag = ":" + crypto.randomUUID();
    ret += loopTag + "\n";
    ret += not(condition) + "/ "+endTag+" truejump\n";
    
    ret += code.join("\n")

    ret += "/ "+loopTag+" jump\n"
    ret += endTag + "\n";
    return ret;
}