from abc import ABC, abstractmethod
from lark import tree, Token, ParseTree

class Variable:
    name:str = ""
    addr:int = 0
    def shift(self, i):
        self.addr += i
    def __init__(self, name:str, addr:int):
        self.name = name
        self.addr = addr
    def toHex(self):
        return hex(self.addr)[2:]
    def __str__(self):
        return "(var {},addr:{})".format(self.name,self.addr)

class Statement(ABC):
    @staticmethod
    def Create(tree:tree.Branch[Token]):
        match tree.data:
            case "call":
                return Call(tree)
            case _:
                raise ValueError("Unknown statement")
    @abstractmethod
    def compile(self, vars:list[Variable]):
        pass

class Expression(ABC):
    @staticmethod
    def Create(tree:tree.Branch[Token]):
        print("EXP:{}".format(tree))
        if hasattr(tree, "type"):
            if tree.type == "SIGNED_NUMBER":
                return Number(tree)
            elif tree.type == "WORD":
                return Var(tree)
        if hasattr(tree, "data"):
            match tree.data:
                case "call":
                    return Call(tree)
                case "add":
                    return Operate(tree)
                case "sub":
                    return Operate(tree)
                case "mul":
                    return Operate(tree)
                case "div":
                    return Operate(tree)
                case "rem":
                    return Operate(tree)
        raise ValueError("Unknown operator")
    @abstractmethod
    def compile(self, vars:list[Variable]):
        pass

class Number(Expression):
    num:int = 0
    def __init__(self, tree: tree.Branch[Token]):
        self.num = int(tree)
    def compile(self, vars:list[Variable]):
        return "/ " + hex(self.num)[2:] + "\n"
    def __str__(self):
        return str(self.num)

class Operate(Expression):
    oprtype:str = ""
    val1:Expression
    val2:Expression
    def __init__(self, tree: tree.Branch[Token]):
        self.oprtype = tree.data
        self.val1 = Expression.Create(tree.children[0])
        self.val2 = Expression.Create(tree.children[1])
    def compile(self, vars: list[Variable]):
        ret = ""
        ret += self.val1.compile(vars)
        ret += self.val2.compile(vars)
        ret += "/ " + self.oprtype + "\n"
        return ret
    def __str__(self):
        return self.oprtype
        #return "{}({},{})".format(self.oprtype,str(self.val1),str(self.val2))

class Var(Expression):
    name:str = ""
    def __init__(self, tree: tree.Branch[Token]):
        self.name = str(tree)
    def compile(self, vars: list[Variable]):
        for var in vars:
            if(var.name == self.name):
                return "/ " + var.toHex() + " load\n"
        raise ValueError("Not found variable " + self.name)
    def __str__(self):
        return self.name

class Call(Expression):
    name:str = ""
    args:list[Expression] = []
    def __init__(self, tree:tree.Branch[Token]):
        self.name = str(tree.children[0])
        for arg in tree.children[1].children:
            if arg != None:
                self.args.append(Expression.Create(arg))
    def compile(self, vars: list[Variable]):
        ret = ""
        for arg in self.args:
            ret += arg.compile(vars)
        
        if self.name == "return":
            ret += "/ ret\n"
        else:
            ret += "/ \\" + self.name + " call\n"
        return ret
    def __str__(self):
        t = "call {}(".format(self.name)
        for arg in self.args:
            t += str(arg) + ","
        t += ")"
        return t

class Root(ABC):
    vars:list[Variable] = []
    def shift(self, i:int):
        for var in self.vars:
            var.shift(i)
    @abstractmethod
    def compile(self, externVal:list[Variable]):
        pass

class Extern(Root):
    isGlobal:bool = True
    def __init__(self, tree:tree.Branch[Token]):
        self.vars = [Variable(str(tree.children[0]),0)]
    def compile(self, externVal):
        return ""
    def __str__(self):
        return "Extern, name:{}, addr:{}\n".format(
            self.vars[0].name, self.vars[0].addr)

class Function(Root):
    name:str = ""
    args:list[str] = []
    states:list[Statement] = []
    def __init__(self, tree:tree.Branch[Token]):
        self.name = str(tree.children[0])
        i = 0
        for arg in tree.children[1].children:
            self.args.append(str(arg))
            self.vars.append(Variable(str(arg), i))
            i += 1
        
        for state in tree.children[2].children:
            if(state.data == "var"):
                self.vars.append(Variable(str(state.children[0].data), i))
                i += 1
            else:
                self.states.append(Statement.Create(state))
    def shift(self, i: int):
        super().shift(i)
        for var in self.vars:
            var.shift(i)
    def compile(self, externVal: list[Variable]):
        allVar = self.vars[:]
        allVar.extend(externVal)
        ret = "export " + self.name + "\n"
        for state in self.states:
            ret += state.compile(allVar)
        return ret
    def __str__(self):
        t = "func {}({}):\n".format(self.name, self.args)
        for state in self.states:
            t += str(state) + "\n"
        return t

class Module:
    items: list[Root] = []
    len:int = 0
    def __init__(self, tree:ParseTree):
        i = 0
        for item in tree.children:
            match item.data:
                case "function":
                    self.items.append(Function(item))
                case "extern":
                    self.items.append(Extern(item))
            self.items[len(self.items)-1].shift(i)
            i += len(self.items[len(self.items)-1].vars)
        self.len = i
    def shift(self, i):
        for item in self.items:
            item.shift(i)
    def compile(self, externVal:list[Variable]):
        ret = ""
        for item in self.items:
            ret += item.compile(externVal)
        return ret
    def __str__(self):
        ret = "Module, var:{}, len:{}\n".format(self.len, len(self.items))
        for item in self.items:
            for t in str(item).split('\n'):
                if t != "":
                    ret += ">> " + t + "\n"
            ret += "\n"
        return ret
                
