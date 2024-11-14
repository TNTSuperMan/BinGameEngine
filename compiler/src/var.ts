let i:Variable = 0;
export type Variable = number;
export const defvar = ():Variable => {
    if(i >= (5 * 4096)-1){
        throw new RangeError("Too many vars")
    }else{
        return i++;
    }
}