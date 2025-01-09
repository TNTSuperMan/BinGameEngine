import { readFileSync } from "node:fs";

export const fmt2bs = (path: string): number[] => {
    const bgesound: number[] = [];

    const fmt = readFileSync(path).toString();

    let Ch = "";
    let lastPos = 0;

    for(const line of fmt.replace("\r","").split("\n")){
        const notereg = /^\t*Note Time=\"(\d+)\" Value="(\w\#\d)" Duration="(\d+)"/
    }

    return bgesound;
}