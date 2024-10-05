<script setup>
import { reactive, ref } from 'vue';

const file = ref()
const is16rad = ref(false)
const filedata = reactive({
  name: "",
  isload: false,
  content: [],
  bin:[]
})
function load(e){
  const f = e.target.files[0]
  const reader = new FileReader()
  filedata.name = f.name
  reader.onload = ()=>{
    const arr = new Uint8Array(reader.result)
    filedata.content = []
    filedata.bin = arr
    for(let i = 0;i < arr.length;i++){
      if(arr[i] == 0){
        filedata.content.push([arr[++i],arr[++i]])
      }else{
        filedata.content.push(arr[i])
      }
    }
  }
  reader.readAsArrayBuffer(f)
}
function operator(e){
  switch(e){
    case 0x00: return "push";
    case 0x01: return "pop";
    case 0x02: return "cls";
    case 0x03: return "pls";
    case 0x04: return "sub";
    case 0x05: return "mul";
    case 0x06: return "div";
    case 0x07: return "rem";
    case 0x08: return "nand";
    case 0x09: return "sin";
    case 0x0a: return "sqrt";
    case 0x0b: return "truejump";
    case 0x0c: return "jump";
    case 0x0d: return "call";
    case 0x0e: return "equal";
    case 0x0f: return "greater";
    case 0x10: return "load";
    case 0x11: return "store";
    case 0x12: return "ret";
    case 0x13: return "redraw";
    case 0x14: return "pixel";
    case 0x15: return "rect";
    case 0x16: return "chkkey";
  }
}
function oid2bid(e){
  let bid = 0;
  for(let i = 0;i < e;i++){
    bid += (Array.isArray(filedata.content[i]) ? 3 : 1)
  }
  return bid
}
</script>

<template>
  <input type="file" v-show="false" ref="file" @change="load">
  <button @click="file.click()">load</button><span>{{ filedata.name }}</span><br>
  16rad:<input type="checkbox" v-model="is16rad">
  <table>
    <tr><th>idx</th><th>bin1</th><th>bin2</th><th>bin3</th><th>|</th><th>operator</th><th>push</th></tr>
    <tr v-for="(d,i) in filedata.content">
      <td>{{ 
        oid2bid(i) + 
        (is16rad ? "(0x" + oid2bid(i).toString(16) + ")" : "")
      }}</td>
      <td>{{ 
        (Array.isArray(d) ? 0 : d) + 
        (is16rad ? "(0x" + (Array.isArray(d) ? 0 : d).toString(16) + ")" : "")
      }}</td>
      <td>{{
        !Array.isArray(d) ? "" : d[0] + (is16rad ? "(0x" + d[0].toString(16) + ")" : "")
      }}</td>
      <td>{{
        !Array.isArray(d) ? "" : d[1] + (is16rad ? "(0x" + d[1].toString(16) + ")" : "")
      }}</td>

      <td class="empty"></td>

      <td>{{ operator(Array.isArray(d) ? 0 : d) }}</td>

      <td v-if="Array.isArray(d)">{{
        d[0] * 256 + d[1] +
        (is16rad ? "(0x" + (d[0] * 256 + d[1]).toString(16) + ")" : "")
      }}</td>
    </tr>
  </table>
</template>

<style scoped>
td{
  margin:0;
  padding:5px;
  border-width:1px 1px 0 0;
  border-color:black;
  border-style:solid;
}
.menu{
  border:1px black solid;
  position:fixed;
  right:0;
  top:0;
}
td.empty{
  border:none;
  width:30px;
}
</style>
