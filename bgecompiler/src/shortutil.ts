import { If } from "./control.ts";
import { defn } from "./fn.ts";
import { add, Expr, greater, num, sub } from "./native.ts";
import { Pointer, Variable, defvar as defBvar, toptr, vr, set, vrP, setP } from "./var.ts";

export type ShortVar = [Variable, Variable];

export const useSutil = ()=>({
    defvar:(description?: string):ShortVar => 
        [defBvar(description), defBvar(description)],
    
    Store:(up:Expr, bot:Expr, v:ShortVar):Expr =>
        set(v[0], up) + set(v[1], bot),
    
    Load:(v:ShortVar):[Expr, Expr] => 
        [vr(v[0]), vr(v[1])],
    
    toPtr:(v:ShortVar):[...Pointer, ...Pointer] => 
        [...toptr(v[0]), ...toptr(v[1])],
    
    
    Add:defn<[Expr,Expr,Expr,Expr,Expr]>("su_add",(addr00,addr01,addr10,addr11,expr)=>
        {
        const addr0:Pointer = [vr(addr00),vr(addr01)];
        const addr1:Pointer = [vr(addr10),vr(addr11)];
        return[
        If(greater(vr(expr), add(sub(vrP(addr1),num(255)),num(1))),[
            setP(addr0, add(vrP(addr1), num(1)))
        ]),
        setP(addr1, add(vrP(addr1), vr(expr)))
    ]}),
    
    Sub:defn<[Expr,Expr,Expr,Expr,Expr]>("su_sub",(addr00,addr01,addr10,addr11,expr)=>
        {
        const addr0:Pointer = [vr(addr00),vr(addr01)];
        const addr1:Pointer = [vr(addr10),vr(addr11)];
        return[
        If(greater(vr(expr), vrP(addr1)),[
            setP(addr0, sub(vrP(addr1), num(1)))
        ]),
        setP(addr1, sub(vrP(addr1), vr(expr)))
    ]}),
})
