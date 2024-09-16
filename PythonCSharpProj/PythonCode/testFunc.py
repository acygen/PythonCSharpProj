import math
import matplotlib.pyplot as plt
import numpy as np
import io
import sys

def TestFunc(arg1, arg2):
    # 生成数据
    x = np.arange(0, 10, 0.1) # 横坐标数据为从0到10之间，步长为0.1的等差数组
    y = np.sin(x) # 纵坐标数据为 x 对应的 sin(x) 值

    fig = plt.figure()
    # 生成图形
    plt.plot(x, y)

    # plt.show()
    # 保存图像
    canvas = fig.canvas
    buffer = io.BytesIO()
    canvas.print_png(buffer)
    data=buffer.getvalue()
    buffer.close()
    arg2(arg1)
    return data

if __name__ == "__main__":
    TestFunc()
