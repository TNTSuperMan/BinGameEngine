from lark import ParseTree
class Root:
    varlen = 0
    addr = 0
    name = None
    def shift(self, i):
        pass
    def compile(self, externVal):
        pass

class Extern(Root):
    varlen = 1
    def __init__(self, name):
        self.name = name
    def shift(self, i):
        self.addr += i
    def compile(self, externVal):
        return "\n"

class Function(Root):
    varlen = 0
    def __init__(self, tree:ParseTree):
        pass#todo:関数の読込処理

class Statement:
    def compile(self, externVal):
        pass
    # todo:ステート分類とこの後