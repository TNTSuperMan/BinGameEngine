import { configureStore, createSlice, PayloadAction, SliceCaseReducers, SliceSelectors } from "@reduxjs/toolkit";

export type Graphic = {
    name: string,
    data: number[][]
}
type State = {data:Graphic[]};
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
                        t.forEach(u=>{
                            if(typeof u != "number") throw 0;
                        })
                    })
                })
                state.data = raw;
            }catch{
                alert("ファイルが不正です")
            }
        }
    }
})

export const store = configureStore({
    reducer: {
        data: slice.reducer
    }
})

export const { init, open } = slice.actions;
