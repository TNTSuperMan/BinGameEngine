import { If } from "control";
import { defn } from "fn";
import { add, Expr, greater, num, set, sub } from "native";
import { Variable, defvar as defBvar, varaddr, vr } from "var";

export type ShortVar = [Variable, Variable];

export const defvar = ():ShortVar => 
    [defBvar(), defBvar()];

export const Store = (v:ShortVar):Expr =>
    `/ ${v[1]} store ${v[0]} store\n`;

export const Load = (v:ShortVar):Expr => 
    vr(v[0]) + vr(v[1])

export const Addr = (v:ShortVar):Expr => 
    varaddr(v[0]) + varaddr(v[1]);


export const Add = defn<[Expr,Expr,Expr]>("su_add",(addr0,addr1,expr)=>[
    If(greater(vr(expr), add(sub(vr(addr1)+"\n/ load\n",num(255)),num(1))),[ // expr > 256-short1 繰り上げ
        set(addr0, add(vr(addr0), num(1)))
    ]),
    set(addr1, add(vr(addr1), vr(expr)))
])

export const Sub = defn<[Expr,Expr,Expr]>("su_sub",(addr0,addr1,expr)=>[
    If(greater(vr(expr), vr(addr1)+"\n/ load\n"),[ // expr > 256-short1 繰り上げ
        set(addr0, sub(vr(addr0), num(1)))
    ]),
    set(addr1, sub(vr(addr1), vr(expr)))
])