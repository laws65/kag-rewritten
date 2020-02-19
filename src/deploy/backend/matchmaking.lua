local nk = require("nakama")

local server_timeout_ms = 60000

local function is_empty(str)
    return str == nil or str == ""
end

local function create_server(context, payload)
    local json = nk.json_decode(payload)

    if not is_empty(json.server_name) and not is_empty(json.server_port) then
        local server = {{
            collection = "servers", 
            key = context.client_ip,
            value = {
                server_ip = context.client_ip,
                server_port = json.server_port,
                server_name = json.server_name,
                last_ping = nk.time()
            }
        }}

        nk.storage_write(server)
    end
end

local function get_servers(context, payload)
    local query_list = nk.storage_list("", "servers", 99, "")
    local result_list = {}
    for _, r in ipairs(query_list) do
        if r.value.last_ping + server_timeout_ms < nk.time() then
            nk.storage_delete({{collection = r.collection, key = r.key}})
        else
            table.insert(result_list, r.value)
        end
    end

    return nk.json_encode(result_list)
end

local function ping(context, payload)
    local query = {{ 
        collection = "servers",
        key = context.client_ip
    }}

    local server = nk.storage_read(query)
    for _, r in ipairs(server) do
        r.value.last_ping = nk.time()
    end
    
    nk.storage_write(server)
end

nk.register_rpc(create_server, "create_server")
nk.register_rpc(get_servers, "get_servers")
nk.register_rpc(ping, "ping")