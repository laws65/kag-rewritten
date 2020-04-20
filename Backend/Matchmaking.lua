local nk = require("nakama")

local server_timeout_sec = 60

local function is_empty(str)
    return str == nil or str == ""
end

local function create_server(context, payload)
    local server_info = nk.json_decode(payload)

    if not is_empty(server_info.Name) then
        local server = {{
            collection = "server_list", 
            key = context.client_ip,
            value = {
                IP = context.client_ip,
                Name = server_info.Name,
                LastPing = nk.time()
            }
        }}

        nk.storage_write(server)
    end
end

local function get_server_list(context, payload)
    local query_list = nk.storage_list("", "server_list", 99, "")
    local result_list = {}
    for _, r in ipairs(query_list) do
        if r.value.LastPing + server_timeout_sec * 1000 < nk.time() then
            nk.storage_delete({{collection = r.collection, key = r.key}})
        else
            table.insert(result_list, r.value)
        end
    end

    return nk.json_encode(result_list)
end

local function ping(context, payload)
    local query = {{ 
        collection = "server_list",
        key = context.client_ip
    }}

    local server = nk.storage_read(query)
    for _, r in ipairs(server) do
        r.value.LastPing = nk.time()
    end
    
    nk.storage_write(server)
end

nk.register_rpc(get_server_list, "get_server_list")
nk.register_rpc(create_server, "create_server")
nk.register_rpc(ping, "ping")
