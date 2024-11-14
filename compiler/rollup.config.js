import typescript from "@rollup/plugin-typescript"
import terser from "@rollup/plugin-terser"
export default {
    input: "./src/index.ts",
    plugins: [typescript(), terser()],
    output: {
        file: "./dist/compiler.min.js"
    }
}