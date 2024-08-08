window.Game=(program, root, clock=0)=>{
    if(!Array.isArray(program)){
        throw "Game function argument 'program' is not array.";
    }
    if(typeof root != "string"){
        throw "Game function argument 'root' is not string."
    }
    //Screen: 100x40
    let key = new libkey()
    let re = document.querySelector(root)
    let pc = 0;
    let screen = [];
    for(let i = 0;i < 40;i++){
        for(let j = 0;j < 100;j++){
            screen.push("@")
        }
        screen.push("\n")
    }
    let reg = new Array(10)
    const nextVal = () => program[pc++]
    let data,x,y,i,rg1,rg2,tjmp,fjmp;
    let isWait = false;
    const frame = setInterval(()=>{
        if(isWait) return;
        if(pc >= program.length) {
            console.log("Complete!")
            clearInterval(frame);
        }
        switch(nextVal()){
            case 2: //JMP
                let ptr = nextVal() * 256;
                ptr += nextVal();
                pc = ptr - 1;
                break;
            case 5: //IF>
                rg1 = reg[nextVal()];
                rg2 = reg[nextVal()];
                tjmp = nextVal() * 256;
                tjmp += nextVal();
                fjmp = nextVal() * 256;
                fjmp += nextVal();
                if(rg1 > rg2){
                    pc = tjmp;
                }else{
                    pc = fjmp;
                }
                break;
            case 7: //IF=
                rg1 = reg[nextVal()];
                rg2 = reg[nextVal()];
                tjmp = nextVal() * 256;
                tjmp += nextVal();
                fjmp = nextVal() * 256;
                fjmp += nextVal();
                if(rg1 == rg2){
                    pc = tjmp;
                }else{
                    pc = fjmp;
                }
                break;
            case 8: //+
                let p1 = reg[nextVal()]
                let p2 = reg[nextVal()]
                reg[nextVal()] = p1 + p2;
                break;
            case 17: //=
                reg[nextVal()] = nextVal()
                break;
            case 18: //PIXEL
                data = String.fromCharCode(reg[nextVal()])
                x = reg[nextVal()]
                y = reg[nextVal()]
                i  = y * 101;
                if(x > 100) break;
                i += x;
                screen[i] = data;
                break;
            case 20: //BOX
                data = String.fromCharCode(reg[nextVal()])
                x = reg[nextVal()]
                y = reg[nextVal()]
                width = reg[nextVal()]
                height = reg[nextVal()]
                for(let dx = 0;dx < width;dx++){
                    for(let dy = 0;dy < height;dy++){
                        i  = (y + dy) * 101;
                        i += (x + dx);
                        screen[i] = data;
                    }
                }
                break;
            case 23: //ISINPUT
                let search = String.fromCharCode(reg[nextVal()]);
                if(key.pressKeyArray.find(e=>e == search)){
                    reg[nextVal()] = 1
                }else{
                    reg[nextVal()] = 0
                }
                break;
            case 26:
                isWait = true;
                setTimeout(e=>{
                    isWait = false;
                },nextVal())
                break;
        }
        let scrtext = ""
        for(let i = 0;i < screen.length;i++){
            scrtext += screen[i]
        }
        re.innerText = scrtext;
    },clock)
    const GameObj = {
        stop:()=>clearInterval(frame)
    };
    Object.defineProperty(GameObj, "frame",{
        get: e=>dc
    })
    Object.defineProperty(GameObj, "pc",{
        get: e=>pc
    })
    window.go = GameObj;
    return GameObj;
}