const t = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXZ0123456789".split("")
export default ()=>"_@"+[0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,].map(e=>t[Math.floor(Math.random()*(t.length-1))]).join("")