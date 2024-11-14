let i = 0;
export const defvar = ():number => {
    if(i >= (5 * 4096)-1){
        throw new RangeError("Too many vars")
    }else{
        return i++;
    }
}