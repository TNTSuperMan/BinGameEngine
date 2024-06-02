document.addEventListener("DOMContentLoaded",e=>{
    let app = Vue.createApp({
        data(){return {funcs:[],loop:[]}},
        mounted:e=>fetch("./funcs.json").then(c=>c.json()).then(obj=>{
            let max = 0;
            Object.entries(obj).forEach(f=>{
                let size = 0;
                f[1].forEach(s=>{size += s.size})
                if(size > max) max = size;
            })
            Object.entries(obj).forEach((func,idx)=>{
                let id = idx.toString(16)
                if(id.length == 1) id = "0" + id;

                let size = 0;
                let emp = [];
                func[1].forEach(s=>{size += s.size})
                for(let i = 0;i < (max - size);i++) emp.push({})
                app.funcs.push({
                    name: func[0],
                    id: id,
                    memory: func[1],
                    emp:emp
                })
            })
            for(let i = 0;i < max;i++) app.loop.push(i + 2);
        })
    }).mount("#p")
})