document.addEventListener("DOMContentLoaded",e=>{
    let _=(256/3)
    let app = Vue.createApp({
        data(){return {r:0,g:0,b:0,cs:[]}},
        computed: {
            style(){
                return `background:rgba(${this.r*_},${this.g*_},${this.b*_},1)`
            }
          }
    }).mount("#p")
    for(let r = 0;r < 4;r++)
        for(let g = 0;g < 4;g++)
            for(let b = 0;b < 4;b++)
                app.cs.push([`background:rgba(${r*_},${g*_},${b*_},1)`,`${r}/${g}/${b}`])
})