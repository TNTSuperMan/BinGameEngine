import { configureStore, createSlice, PayloadAction, SliceCaseReducers, SliceSelectors } from "@reduxjs/toolkit";

export type Graphic = {
    name: string,
    data: number[][]
}
export type State = {data:Graphic[]};
const slice = createSlice<State, SliceCaseReducers<State>, string, SliceSelectors<State>>({
    name: "data",
    initialState: {data:[]},
    reducers: {
        init(state){
            state.data = [];
        },
        open(state, action: PayloadAction<string>){
            try{ //File typecheck
                const raw = JSON.parse(action.payload);
                if(!Array.isArray(raw)) throw 0;
                raw.forEach(e=>{
                    if(typeof e != "object") throw 0;
                    if(typeof e.name != "string") throw 0;
                    const d = e.data;
                    if(!Array.isArray(d)) throw 0;
                    d.forEach(t=>{
                        if(!Array.isArray(t)) throw 0;
                        t.forEach(e=>{
                            if(typeof e != "number") throw 0;
                        })
                    })
                })
                state.data = raw;
            }catch{
                alert("ファイルが不正です")
            }
        },
        edit(state, action: PayloadAction<[number, number[][]]>){
            state.data[action.payload[0]].data = action.payload[1];
        },
        editPixel(state, action: PayloadAction<[number, number, number, number]>){
            state.data[action.payload[0]].data[action.payload[1]][action.payload[2]] = action.payload[3];
        }
    }
})

export const store = configureStore({
    reducer: {
        data: slice.reducer
    }
})

export const { init, open, edit, editPixel } = slice.actions;
