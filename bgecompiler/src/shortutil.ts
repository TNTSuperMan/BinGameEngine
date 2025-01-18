import { If } from "./control";
import { defn } from "./fn";
import { add, Expr, greater, num, ret, sub } from "./native";
import { Pointer, Variable, defvar as defBvar, toptr, vr, set, vrP, setP } from "./var";
import genid from "genid";

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
    
    fromTag:(e:Expr,v:ShortVar):Expr =>
        set(v[0], `${e}\n/ pop\n`)+
        set(v[1], `${e}\n`)+"/ pop\n",
    
    Add:defn<[Expr,Expr,Expr,Expr,Expr]>("su_add_"+genid(),(addr00,addr01,addr10,addr11,expr)=>
        {
        const addr0:Pointer = [vr(addr00),vr(addr01)];
        const addr1:Pointer = [vr(addr10),vr(addr11)];
        return[
        If(greater(vr(expr), sub(num(255),vrP(addr1))),[
            setP(addr0, add(vrP(addr0), num(1)))
        ]),
        setP(addr1, add(vrP(addr1), vr(expr))),
        ret()
    ]}),
    
    Sub:defn<[Expr,Expr,Expr,Expr,Expr]>("su_sub_"+genid(),(addr00,addr01,addr10,addr11,expr)=>
        {
        const addr0:Pointer = [vr(addr00),vr(addr01)];
        const addr1:Pointer = [vr(addr10),vr(addr11)];
        return[
        If(greater(vr(expr), vrP(addr1)),[
            setP(addr0, sub(vrP(addr0), num(1)))
        ]),
        setP(addr1, sub(vrP(addr1), vr(expr))),
        ret()
    ]}),
})
