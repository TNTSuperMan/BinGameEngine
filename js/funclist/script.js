document.addEventListener("DOMContentLoaded",e=>{
    let app = Vue.createApp({
        data(){return {funcs:[],loop:[]}},
        mounted:e=>fetch("./funcs.json").then(c=>c.json()).then(obj=>{
            let max = 0;
            Object.entries(obj).forEach(f=>{
                if(f[1].length > max) max = f[1].length;
            })
            Object.entries(obj).forEach((func,idx)=>{
                let id = idx.toString(16)
                if(id.length == 1) id = "0" + id;

                app.funcs.push({
                    name: func[0],
                    id: id,
                    memory: func[1],
                    i:idx
                })
            })
            for(let i = 0;i < max;i++) app.loop.push(i + 1);
        })
    }).mount("#p")
})