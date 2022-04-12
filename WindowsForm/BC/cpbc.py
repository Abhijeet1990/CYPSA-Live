import csv
import pandas as pd 
import networkx as nx
import matplotlib.pyplot as plt
import numpy as np
from datetime import datetime

from collections import defaultdict

def node_importance_function(nodes, G, Internet_nodes, relay_nodes):
    node_importance = {}
    node_importance = defaultdict(lambda: 0.0, node_importance)

    from datetime import datetime
    for j in range(len(Internet_nodes)):
        for k in range(len(relay_nodes)):
            if Internet_nodes[j] == relay_nodes[k]:
                break

            search_short_path = nx.all_shortest_paths(G, Internet_nodes[j], relay_nodes[k], weight='weight', method='dijkstra')
            try:
                #print(list(search_short_path))
                short_paths = list(search_short_path)
                for short_path in short_paths:
                    for i in range(len(nodes)):
                        if (all(x in short_path for x in [nodes[i]]) ==True):
                            #ind = nodes.index(node)
                            #print(ind)
                            node_importance[nodes[i]] +=1
            except Exception as e:
                pass
    return dict(node_importance)

def node_importance_function_cpbc(nodes, G, Internet_nodes, relay_nodes):
    node_importance = {} 
    node_importance = defaultdict(lambda:0.0,node_importance)
    #node_importance = [0.0]*len(nodes)
    short_path_count_1 = []
    
    start_time = datetime.now()
    for j in range(len(Internet_nodes)):
        for k in range(len(relay_nodes)):
            if Internet_nodes[j] == relay_nodes[k]:
                break

            search_short_path = nx.all_shortest_paths(G, Internet_nodes[j], relay_nodes[k], weight='weight', method='dijkstra')
            try:
                #print(list(search_short_path))
                short_paths = list(search_short_path)
                short_path_count_1.append(len(short_paths))
                for short_path in short_paths:
                    for i in range(len(nodes)):
                        if (all(x in short_path for x in [nodes[i]]) ==True):
                            #ind = nodes.index(node)
                            #print(ind)
                            node_importance[nodes[i]] +=1
            except Exception as e:
                pass
    end_time = datetime.now()        
    return dict(node_importance), short_path_count_1


def node_importance_cpbc(node_list, cost_list, node_importance_dict):
    node_importance_cpbc = {}
    node_importance_cpbc = defaultdict(lambda: 0.0, node_importance_cpbc)
    #node_importance_cpbc = [0.0] * len(nodes)
    node_importance = list(node_importance_dict.values())
    for i in range(len(node_importance)):
        node_importance_1 = (node_importance[i] - np.min(np.array(node_importance))) / (
                    np.max(np.array(node_importance)) - np.min(np.array(node_importance)))
        node_list_1 = (node_list[i] - np.min(np.array(node_list))) / (
                    np.max(np.array(node_list)) - np.min(np.array(node_list)))
        cost_list_1 = (cost_list[i] - np.min(np.array(cost_list))) / (
                    np.max(np.array(cost_list)) - np.min(np.array(cost_list)))
        try:
            node_importance_cpbc_ev = (node_importance_1) + (node_list_1 * 1 / (1 / (cost_list_1)))  # the new extended metric
            node_importance_cpbc[list(node_importance_dict.keys())[i]] = node_importance_cpbc_ev
        except:
            #print('Error')
            continue


    return node_importance_cpbc

def cardinality_cc(nodes, start_nodes, destination_nodes, weights):
    node_list = [0.0]*len(nodes)
    cost_list =[0.0]*len(nodes)
    for j in range(len(nodes)):
        for i in range(len(start_nodes)):
            if int(nodes[j]) == int(start_nodes[i]):
                node_list[j] +=1
                cost_list[j] += weights[i]
            elif int(nodes[j]) == int(destination_nodes[i]):
                node_list[j] +=1
                cost_list[j] += weights[i]
            else:
                pass

    return node_list, cost_list

#print('Hello#World,WOrld#Hello')
my_filtered_csv = pd.read_csv("C:\\Users\\substationc\\Desktop\\cypsa_live\\WindowsForm\\BC\\amara.csv")
some_list = my_filtered_csv['internet_node_id'].tolist()
Internet_nodes = [x for x in some_list if str(x) != 'nan']
relay_nodes = [x for x in my_filtered_csv['relay_node_id'].tolist() if str(x) != 'nan']
weights = [x for x in my_filtered_csv['Cost'].tolist() if str(x) != 'nan']
start_nodes = [x for x in my_filtered_csv['From'].tolist() if str(x) != 'nan']
destination_nodes = [x for x in my_filtered_csv['To'].tolist() if str(x) != 'nan']


G = nx.MultiGraph()
e = zip(start_nodes,destination_nodes,weights)
G.add_edges_from(e)
#nx.draw_networkx(G,node_size=10, with_labels=False)
nodes=np.unique([start_nodes,destination_nodes])

node_list, cost_list = cardinality_cc(nodes, start_nodes, destination_nodes, weights)
node_importance = node_importance_function(nodes, G, Internet_nodes, relay_nodes)
node_imp_cpbc = node_importance_cpbc(node_list,cost_list,node_importance)
node_imp_bc = node_importance_function(nodes, G, Internet_nodes, relay_nodes)
cpbc_vals=[]
bc_vals=[]
#print('Hello#World,WOrld#Hello')
for k,v in node_imp_cpbc.items():
    cpbc_vals.append(str(k)+'#'+str(v))

for k,v in node_imp_bc.items():
    bc_vals.append(str(k)+'#'+str(v))
print(','.join(bc_vals)+'*'+','.join(cpbc_vals))