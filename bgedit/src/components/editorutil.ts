export const TRANSPARENT = 0b01000000;
export const NEXTLINE = 0b10000000;

export const isNextLine = (e:number) => (0b10000000 & e) == NEXTLINE;
