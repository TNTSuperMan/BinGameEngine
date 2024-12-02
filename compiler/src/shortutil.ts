import { If } from "./control.ts";
import { defn } from "./fn.ts";
import { add, Expr, greater, num, set, sub } from "./native.ts";
import { Variable, defvar as defBvar, varaddr, vr } from "./var.ts";

export type ShortVar = [Variable, Variable];

export default ()=>({
    defvar:():ShortVar => 
        [defBvar(), defBvar()],
    
    Store:(v:ShortVar):Expr =>
        (`/ ${v[1]} store ${v[0]} store\n`),
    
    Load:(v:ShortVar):Expr => 
        vr(v[0]) + vr(v[1]),
    
    Addr:(v:ShortVar):Expr => 
        varaddr(v[0]) + varaddr(v[1]),
    
    
    Add:defn<[Expr,Expr,Expr]>("su_add",(addr0,addr1,expr)=>[
        If(greater(vr(expr), add(sub(vr(addr1)+"\n/ load\n",num(255)),num(1))),[ // expr > 256-short1 繰り上げ
            set(addr0, add(vr(addr0), num(1)))
        ]),
        set(addr1, add(vr(addr1), vr(expr)))
    ]),
    
    Sub:defn<[Expr,Expr,Expr]>("su_sub",(addr0,addr1,expr)=>[
        If(greater(vr(expr), vr(addr1)+"\n/ load\n"),[ // expr > 256-short1 繰り上げ
            set(addr0, sub(vr(addr0), num(1)))
        ]),
        set(addr1, sub(vr(addr1), vr(expr)))
    ])
})
